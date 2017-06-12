package com.drive.app.fragments;


import android.Manifest;
import android.app.Fragment;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.location.Location;
import android.os.Bundle;
import android.os.Handler;
import android.preference.PreferenceManager;
import android.support.v4.app.ActivityCompat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.Event;
import com.drive.app.model.EventsNearbyFilter;
import com.drive.app.model.User;
import com.drive.app.model.UsersNearbyFilter;
import com.drive.app.rest.RetrofitClient;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapView;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.model.BitmapDescriptorFactory;
import com.google.android.gms.maps.model.CameraPosition;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.ViewById;

import java.io.IOException;
import java.util.List;

import retrofit.Call;
import retrofit.Response;

@EFragment(R.layout.fragment_map)
public class AppMapFragment extends Fragment implements OnMapReadyCallback {

    @ViewById(R.id.mapView)
    MapView mMapView;
    @Bean
    RetrofitClient retrofitClient;

    private SharedPreferences sharedPreferences;
    private Context context;
    private Bundle savedInstanceState;
    private boolean firstLocationUpdate = true;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        this.savedInstanceState = savedInstanceState;
        return null;
    }

    @AfterViews
    void init() {
        mMapView.onCreate(savedInstanceState);
        mMapView.getMapAsync(this);
        context = getActivity().getApplicationContext();
        sharedPreferences = PreferenceManager.getDefaultSharedPreferences(context);
    }

    @SuppressWarnings("deprecation")
    @Override
    public void onMapReady(final GoogleMap mMap) {
        if (ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
            mMap.setMyLocationEnabled(true);
            GoogleMap.OnMyLocationChangeListener myLocationChangeListener = new GoogleMap.OnMyLocationChangeListener() {
                @Override
                public void onMyLocationChange(Location location) {
                    if (firstLocationUpdate) {
                        LatLng loc = new LatLng(location.getLatitude(), location.getLongitude());
                        CameraPosition cameraPosition = new CameraPosition.Builder()
                                .target(loc).zoom(12).build();
                        mMap.animateCamera(CameraUpdateFactory
                                .newCameraPosition(cameraPosition));
                        firstLocationUpdate = false;
                        getPoints(loc, mMap);
                    }
                }
            };
            mMap.setOnMyLocationChangeListener(myLocationChangeListener);
        }

    }

    @Background
    void getPoints(LatLng loc, final GoogleMap googleMap) {
        try {
            String pointsType = sharedPreferences.getString("settingsShowOnMap", "all");
            boolean isError = false;
            RetrofitClient.ApiService service = retrofitClient.getApiService();

            if (pointsType.equals("all") || pointsType.equals("users")) {
                int distance = Integer.parseInt(sharedPreferences.getString("settingsMapRange", "20"));
                Call<List<User>> getUsersCall = service.getUsersNearby(new UsersNearbyFilter(distance, loc.latitude, loc.longitude));
                Response<List<User>> response = getUsersCall.execute();
                if (response != null) {
                    switch (response.code()) {
                        case 200:
                            for (User user : response.body()) {
                                String vehicle = "";
                                if (user.getVehicleMake() != null && user.getVehicleModel() != null) {
                                    vehicle = user.getVehicleMake() + " " + user.getVehicleModel();
                                }
                                final MarkerOptions marker = new MarkerOptions().position(
                                        new LatLng(user.getLatitude(), user.getLongitude())).title(user.getUserName()).snippet(vehicle);
                                marker.icon(BitmapDescriptorFactory
                                        .defaultMarker(BitmapDescriptorFactory.HUE_ROSE));
                                Handler mainHandler = new Handler(context.getMainLooper());
                                Runnable myRunnable = new Runnable() {
                                    @Override
                                    public void run() {
                                        googleMap.addMarker(marker);
                                    }
                                };
                                mainHandler.post(myRunnable);
                            }
                            break;
                        default:
                            isError = true;
                            break;
                    }
                } else isError = true;
            }

            if (pointsType.equals("all") || pointsType.equals("events")) {
                int daysMax = Integer.parseInt(sharedPreferences.getString("settingsDayEventsRange", "7"));
                int eventsDistanceMax = Integer.parseInt(sharedPreferences.getString("settingsMapEventsRange", "20"));
                Call<List<Event>> getEventsCall = service.getEventsNearby(new EventsNearbyFilter(daysMax, eventsDistanceMax, loc.latitude, loc.longitude));
                Response<List<Event>> eventsResponse = getEventsCall.execute();
                if (eventsResponse != null) {
                    switch (eventsResponse.code()) {
                        case 200:
                            for (Event event : eventsResponse.body()) {
                                final MarkerOptions marker = new MarkerOptions().position(
                                        new LatLng(event.getLatitude(), event.getLongitude())).title(event.getName());
                                marker.icon(BitmapDescriptorFactory
                                        .defaultMarker(BitmapDescriptorFactory.HUE_CYAN));
                                Handler mainHandler = new Handler(context.getMainLooper());
                                Runnable myRunnable = new Runnable() {
                                    @Override
                                    public void run() {
                                        googleMap.addMarker(marker);
                                    }
                                };
                                mainHandler.post(myRunnable);
                            }
                            break;
                        default:
                            showToast(context.getString(R.string.map_points_loading_error));
                            break;
                    }
                }
            }

            if (isError) throw new IOException("Failed to get map points.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("get_map_points_error", e.getMessage());
            showToast(context.getString(R.string.map_points_loading_error));
        }
    }

    private void showToast(final String message) {
        Handler mainHandler = new Handler(context.getMainLooper());
        Runnable myRunnable = new Runnable() {
            @Override
            public void run() {
                Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
            }
        };
        mainHandler.post(myRunnable);
    }

    @Override
    public void onResume() {
        super.onResume();
        mMapView.onResume();
    }

    @Override
    public void onPause() {
        super.onPause();
        mMapView.onPause();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        mMapView.onDestroy();
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        mMapView.onLowMemory();
    }
}