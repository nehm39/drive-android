package com.drive.app.activities;

import android.Manifest;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Handler;
import android.preference.PreferenceManager;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.app.ActivityCompat;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.fragments.AppMapFragment_;
import com.drive.app.fragments.EventsFragment_;
import com.drive.app.fragments.NewsfeedFragment_;
import com.drive.app.fragments.ProfileFragment;
import com.drive.app.fragments.ProfileFragment_;
import com.drive.app.fragments.SettingsFragment;
import com.drive.app.fragments.UsersFragment_;
import com.drive.app.model.APIStatus;
import com.drive.app.model.GeoPoint;
import com.drive.app.prefs.LoginPrefs_;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.androidannotations.annotations.sharedpreferences.Pref;
import org.androidannotations.api.BackgroundExecutor;

import java.io.IOException;

import retrofit.Call;
import retrofit.Response;

/**
 * Created by Szymon Gajewski on 11.12.2015.
 */
@EActivity(R.layout.activity_main)
public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener, ProfileFragment.OnProfileChangedListener {

    private static final int LOCATION_PERMISSIONS_MAP_REQUEST_CODE = 5;
    private static final int LOCATION_PERMISSIONS_MAIN_REQUEST_CODE = 6;
    private static final String LOCATION_UPDATE_TASK_NAME = "location_update_task";
    private int currentDrawerPosition = R.id.drawer_option_newsfeed;

    @ViewById(R.id.toolbar)
    Toolbar toolbar;
    @ViewById(R.id.drawer_layout)
    DrawerLayout drawer;
    @ViewById(R.id.nav_view)
    NavigationView navigationView;

    @Pref
    LoginPrefs_ loginPrefs;
    @Bean
    RetrofitClient retrofitClient;

    private boolean backPressed;
    private Location currentLocation;

    @AfterViews
    void init() {
        setSupportActionBar(toolbar);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();
        navigationView.setNavigationItemSelectedListener(this);
        getFragmentManager().beginTransaction()
                .replace(R.id.fragment_container, new NewsfeedFragment_(), "newsfeed")
                .commit();
        navigationView.getMenu().findItem(R.id.drawer_option_newsfeed).setChecked(true);

        setDrawerHeaderNames(loginPrefs.userMail().getOr(""), loginPrefs.userName().getOr(""));

        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
            setLocationListener();
        } else ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION}, LOCATION_PERMISSIONS_MAIN_REQUEST_CODE);
    }

    @SuppressWarnings("ResourceType")
    private void setLocationListener() {
        LocationManager locationManager = (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);
        LocationListener locationListener = new LocationListener() {
            public void onLocationChanged(Location location) {
                currentLocation = location;
            }
            public void onStatusChanged(String provider, int status, Bundle extras) {
            }
            public void onProviderEnabled(String provider) {
            }
            public void onProviderDisabled(String provider) {
            }
        };
        locationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 0, 0, locationListener);
    }

    @Override
    public void onBackPressed() {
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            if (backPressed) {
                Intent intent = new Intent(Intent.ACTION_MAIN);
                intent.addCategory(Intent.CATEGORY_HOME);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                startActivity(intent);
            } else {
                Toast.makeText(getApplicationContext(), getString(R.string.app_back), Toast.LENGTH_SHORT).show();
                backPressed = true;
            }
        }
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        int id = item.getItemId();
        FragmentManager fragmentManager = getFragmentManager();
        Fragment fragment = null;
        String tag = "";

        switch (id) {
            case R.id.drawer_option_newsfeed:
                fragment = new NewsfeedFragment_();
                break;
            case R.id.drawer_option_map:
                if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                    ActivityCompat.requestPermissions(this,
                            new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION}, LOCATION_PERMISSIONS_MAP_REQUEST_CODE);
                } else fragment = new AppMapFragment_();
                break;
            case R.id.drawer_option_events:
                fragment = new EventsFragment_();
                break;
            case R.id.drawer_option_users:
                fragment = new UsersFragment_();
                break;
            case R.id.drawer_option_profile:
                fragment = new ProfileFragment_();
                break;
            case R.id.drawer_option_settings:
                fragment = new SettingsFragment();
                break;
            case R.id.drawer_option_logout:
                loginPrefs.clear();
                LoginActivity_.intent(this).start();
                break;
        }

        if (fragment != null) {
            fragmentManager.beginTransaction()
                    .replace(R.id.fragment_container, fragment, tag)
                    .commit();
            currentDrawerPosition = id;
        }

        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String permissions[], @NonNull int[] grantResults) {
        switch (requestCode) {
            case LOCATION_PERMISSIONS_MAP_REQUEST_CODE:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    new Handler().postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            Fragment fragment = new AppMapFragment_();
                            getFragmentManager().beginTransaction()
                                    .replace(R.id.fragment_container, fragment)
                                    .commit();
                        }
                    }, 200);
                } else {
                    Toast.makeText(this, getString(R.string.location_permissions_deny), Toast.LENGTH_SHORT).show();
                    navigationView.getMenu().findItem(currentDrawerPosition).setChecked(true);
                }
                break;
            case LOCATION_PERMISSIONS_MAIN_REQUEST_CODE:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    setLocationListener();
                }
                break;
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        if (PreferenceManager.getDefaultSharedPreferences(this).getBoolean("settingsAutomaticUpdate", false)) {
            updateLocation();
            updateLocationTask();
        }
    }

    @SuppressWarnings("InfiniteRecursion")
    @Background(delay=60000, id=LOCATION_UPDATE_TASK_NAME)
    void updateLocationTask() {
        if (PreferenceManager.getDefaultSharedPreferences(this).getBoolean("settingsAutomaticUpdate", false)) {
            updateLocation();
            updateLocationTask();
        }
    }

    @Background
    void updateLocation() {
        if (currentLocation != null) {
            try {
                RetrofitClient.ApiService service = retrofitClient.getApiService();
                Call<APIStatus> updateLocationCall = service.updateUserLocation(new GeoPoint(currentLocation.getLatitude(), currentLocation.getLongitude()));
                Response<APIStatus> response = updateLocationCall.execute();
                if (response == null) throw new IOException("Failed to update location.");
            } catch (IOException e) {
                if (e.getMessage() != null) Log.e("location_update_error", e.getMessage());
            }
        }
    }

    @Override
    public void onStop() {
        super.onStop();
        BackgroundExecutor.cancelAll(LOCATION_UPDATE_TASK_NAME, true);
    }

    @Override
    public void onDataChanged(String userName, String userMail) {
        setDrawerHeaderNames(userName, userMail);
    }

    private void setDrawerHeaderNames(String userName, String userMail) {
        View header = navigationView.getHeaderView(0);
        TextView headerUserMail = (TextView) header.findViewById(R.id.drawer_header_user_mail);
        TextView headerUserName = (TextView) header.findViewById(R.id.drawer_header_user_name);
        headerUserMail.setText(userName);
        headerUserName.setText(userMail);
    }
}
