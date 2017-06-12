package com.drive.app.other;

import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.drive.app.R;

/**
 * Created by Szymon Gajewski on 11.01.2016.
 */
public class EventHolder extends RecyclerView.ViewHolder {

    final TextView txtDate;
    final TextView txtName;
    final TextView txtDesc;
    final TextView txtCity;
    final ImageView image;
    final ProgressBar imageProgressBar;
    int id;

    public EventHolder(View view) {
        super(view);
        this.txtName = (TextView) view.findViewById(R.id.eventTitle);
        this.txtDesc = (TextView) view.findViewById(R.id.eventDesc);
        this.txtDate = (TextView) view.findViewById(R.id.eventDate);
        this.txtCity = (TextView) view.findViewById(R.id.eventCity);
        this.image = (ImageView) view.findViewById(R.id.eventImage);
        this.imageProgressBar = (ProgressBar) view.findViewById(R.id.imageProgressBar);
    }
}
