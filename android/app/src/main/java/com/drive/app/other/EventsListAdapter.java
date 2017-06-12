package com.drive.app.other;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.target.Target;
import com.drive.app.R;
import com.drive.app.model.Event;
import com.drive.app.rest.RetrofitClient;

import org.joda.time.DateTime;
import org.joda.time.format.DateTimeFormat;
import org.joda.time.format.DateTimeFormatter;

import java.util.List;

/**
 * Created by Szymon Gajewski on 11.01.2016.
 */
public class EventsListAdapter extends RecyclerView.Adapter<EventHolder> {
    private final List<Event> eventsList;

    public EventsListAdapter(List<Event> eventsList) {
        this.eventsList = eventsList;
    }

    @Override
    public EventHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.events_row, parent, false);
        return new EventHolder(view);
    }

    @Override
    public void onBindViewHolder(final EventHolder holder, int position) {
        Event event = eventsList.get(position);
        holder.imageProgressBar.setVisibility(View.VISIBLE);
        holder.id = event.getId();
        holder.txtName.setText(event.getName());
        holder.txtDesc.setText(event.getDescription());
        holder.txtCity.setText(event.getCity());
        DateTime dt = new DateTime(event.getDate()*1000);
        DateTimeFormatter dtf = DateTimeFormat.shortDateTime();
        holder.txtDate.setText(dtf.print(dt));
        String imageUrl = RetrofitClient.API_EVENTS_IMAGES_URL + event.getId() + ".jpg";
        Glide.with(holder.txtCity.getContext()).load(imageUrl).listener(new RequestListener<String, GlideDrawable>() {
            @Override
            public boolean onException(Exception e, String model, Target<GlideDrawable> target, boolean isFirstResource) {
                return false;
            }

            @Override
            public boolean onResourceReady(GlideDrawable resource, String model, Target<GlideDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                holder.imageProgressBar.setVisibility(View.GONE);
                return false;
            }
        }).into(holder.image);
    }

    @Override
    public int getItemCount() {
        return eventsList.size();
    }
}
