package com.drive.app.other;

import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.TextView;

import com.drive.app.R;

/**
 * Created by Szymon Gajewski on 18.01.2016.
 */
public class UserHolder extends RecyclerView.ViewHolder {

    final TextView txtName;
    final TextView txtVehicle;
    final TextView txtCity;
    final TextView txtMail;
    int id;

    public UserHolder(View view) {
        super(view);
        this.txtName = (TextView) view.findViewById(R.id.userName);
        this.txtVehicle = (TextView) view.findViewById(R.id.userVehicle);
        this.txtCity = (TextView) view.findViewById(R.id.userCity);
        this.txtMail = (TextView) view.findViewById(R.id.userMail);
    }
}
