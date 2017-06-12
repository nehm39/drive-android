package com.drive.app.fragments;


import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.APIStatus;
import com.drive.app.model.User;
import com.drive.app.prefs.LoginPrefs_;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import org.androidannotations.annotations.sharedpreferences.Pref;

import java.io.IOException;

import retrofit.Call;
import retrofit.Response;

@EFragment(R.layout.fragment_profile)
public class ProfileFragment extends Fragment {

    @Pref
    LoginPrefs_ loginPrefs;
    @Bean
    RetrofitClient retrofitClient;
    @ViewById(R.id.etxt_profile_mail)
    EditText etxtMail;
    @ViewById(R.id.etxt_profile_password)
    EditText etxtPassword;
    @ViewById(R.id.etxt_profile_place)
    EditText etxtPlace;
    @ViewById(R.id.etxt_profile_vehicle_make)
    EditText etxtVehicleMake;
    @ViewById(R.id.etxt_profile_vehicle_model)
    EditText etxtVehicleModel;
    @ViewById(R.id.profile_main_layout)
    RelativeLayout mainLayout;
    @ViewById(R.id.profile_progress_bar)
    ProgressBar progressBar;

    @Click(R.id.profile_button_save)
    void onProfileUpdateButtonClick() {
        updateUser();
    }

    private Context context;

    public ProfileFragment() {
    }

    @AfterViews
    void init() {
        context = getActivity().getApplicationContext();
        etxtPassword.setText(loginPrefs.password().getOr(""));
        getUser();
    }

    @Background
    void getUser() {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            Call<User> getUserForProfile = service.getUserForProfile();
            Response<User> response = getUserForProfile.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        User user = response.body();
                        updateUserUi(user);
                        break;
                    default:
                        showToast(context.getString(R.string.profile_get_error));
                        break;
                }
            } else throw new IOException("Failed to get user.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("get_user_error", e.getMessage());
            showToast(context.getString(R.string.profile_get_error));
        }
    }

    @Background
    void updateUser() {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            User user = new User();
            user.setCity(etxtPlace.getText().toString());
            user.setMail(etxtMail.getText().toString());
            user.setPassword(etxtPassword.getText().toString());
            user.setUserName(loginPrefs.userName().get());
            user.setVehicleMake(etxtVehicleMake.getText().toString());
            user.setVehicleModel(etxtVehicleModel.getText().toString());
            Call<APIStatus> updateUser = service.updateUser(user);
            Response<APIStatus> response = updateUser.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        APIStatus status = response.body();
                        updateUserUi(user);
                        loginPrefs.edit().password().put(user.getPassword());
                        showToast(status.getMessage());
                        callMainActivityProfileCallback(user.getUserName(), user.getMail());
                        break;
                    default:
                        showToast(context.getString(R.string.profile_update_error));
                        break;
                }
            } else throw new IOException("Failed to update user.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("update_user_error", e.getMessage());
            showToast(context.getString(R.string.profile_update_error));
        }
    }

    @UiThread
    void updateUserUi(User user) {
        etxtMail.setText(user.getMail());
        etxtPlace.setText(user.getCity());
        if (user.getPassword() != null && !user.getPassword().isEmpty()) etxtPassword.setText(user.getPassword());
        if (user.getVehicleMake() != null && !user.getVehicleMake().isEmpty()) etxtVehicleMake.setText(user.getVehicleMake());
        if (user.getVehicleModel() != null && !user.getVehicleModel().isEmpty()) etxtVehicleModel.setText(user.getVehicleModel());
        progressBar.setVisibility(View.GONE);
        mainLayout.setVisibility(View.VISIBLE);
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

    OnProfileChangedListener profileCallback;

    public interface OnProfileChangedListener {
        public void onDataChanged(String userName, String userMail);
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        Activity activity = (Activity) context;
        try {
            profileCallback = (OnProfileChangedListener) activity;
        } catch (ClassCastException e) {
            Log.e("profile_on_attach", "OnProfileChangedListener not implemented");
        }
    }

    @UiThread
    void callMainActivityProfileCallback(String userName, String userMail) {
        profileCallback.onDataChanged(userName, userMail);
}

}
