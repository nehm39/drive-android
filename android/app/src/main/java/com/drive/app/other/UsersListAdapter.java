package com.drive.app.other;

import android.annotation.SuppressLint;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.drive.app.R;
import com.drive.app.model.User;

import java.util.List;

/**
 * Created by Szymon Gajewski on 18.01.2016.
 */
public class UsersListAdapter extends RecyclerView.Adapter<UserHolder> {
    private final List<User> usersList;

    public UsersListAdapter(List<User> usersList) {
        this.usersList = usersList;
    }

    @Override
    public UserHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.users_row, parent, false);
        return new UserHolder(view);
    }

    @SuppressLint("SetTextI18n")
    @Override
    public void onBindViewHolder(final UserHolder holder, int position) {
        User user = usersList.get(position);
        holder.id = user.getId();
        holder.txtName.setText(user.getUserName());
        holder.txtCity.setText(user.getCity());
        if (user.getVehicleMake() != null && user.getVehicleModel() != null) holder.txtVehicle.setText(user.getVehicleMake() + " " + user.getVehicleModel());
        else holder.txtVehicle.setText("");
        if (user.getMail() != null) holder.txtMail.setText(user.getMail());
    }

    @Override
    public int getItemCount() {
        return usersList.size();
    }
}