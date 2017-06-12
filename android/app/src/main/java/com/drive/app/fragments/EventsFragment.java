package com.drive.app.fragments;


import android.app.Fragment;
import android.content.Context;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.Event;
import com.drive.app.other.EventsListAdapter;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;

import java.io.IOException;
import java.util.List;

import retrofit.Call;
import retrofit.Response;

@EFragment(R.layout.fragment_events)
public class EventsFragment extends Fragment {

    @Bean
    RetrofitClient retrofitClient;

    @ViewById(R.id.events_recycler_view)
    RecyclerView recyclerView;
    @ViewById(R.id.events_main_layout)
    RelativeLayout mainLayout;
    @ViewById(R.id.events_progress_bar)
    ProgressBar progressBar;

    private Context context;

    public EventsFragment() {
    }

    @AfterViews
    void init() {
        setHasOptionsMenu(true);
        context = getActivity().getApplicationContext();
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        getEvents();
    }

        @Background
        void getEvents() {
            try {
                RetrofitClient.ApiService service = retrofitClient.getApiService();
                Call<List<Event>> getEvents = service.getAllEvents();
                Response<List<Event>> response = getEvents.execute();
                if (response != null) {
                    switch (response.code()) {
                        case 200:
                            List<Event> events = response.body();
                            setAdapter(events);
                            break;
                        default:
                            showToast(context.getString(R.string.general_error_with_retry));
                            break;
                    }
                } else throw new IOException("Failed to get events.");
            } catch (IOException e) {
                if (e.getMessage() != null) Log.e("get_events_error", e.getMessage());
                showToast(context.getString(R.string.general_error_with_retry));
            }
        }


    @UiThread
    void setAdapter(List<Event> events) {
        EventsListAdapter eventsListAdapter = new EventsListAdapter(events);
        recyclerView.setAdapter(eventsListAdapter);
        progressBar.setVisibility(View.INVISIBLE);
        mainLayout.setVisibility(View.VISIBLE);
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

}
