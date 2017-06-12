package com.drive.app.activities;

import android.content.Context;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.EditText;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.APIStatus;
import com.drive.app.model.User;
import com.drive.app.prefs.LoginPrefs_;
import com.drive.app.rest.RetrofitClient;
import com.squareup.okhttp.ResponseBody;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import org.androidannotations.annotations.sharedpreferences.Pref;

import java.io.IOException;
import java.lang.annotation.Annotation;

import retrofit.Call;
import retrofit.Converter;
import retrofit.Response;

@EActivity(R.layout.activity_register)
public class RegisterActivity extends AppCompatActivity {
    private Context context;
    @Pref
    LoginPrefs_ loginPrefs;
    @Bean
    RetrofitClient retrofitClient;

    @ViewById(R.id.register_etxt_username)
    EditText etxtUsername;
    @ViewById(R.id.register_etxt_password)
    EditText etxtPassword;
    @ViewById(R.id.register_etxt_email)
    EditText etxtEmail;
    @ViewById(R.id.register_etxt_location)
    EditText etxtLocation;
    @ViewById(R.id.register_etxt_vehicle_make)
    EditText etxtVehicleMake;
    @ViewById(R.id.register_etxt_vehicle_model)
    EditText etxtVehicleModel;

    @Click(R.id.register_btn_reg)
    void registerButtonClicked() {
        if (!etxtPassword.getText().toString().isEmpty() && !etxtUsername.getText().toString().isEmpty() &&
                !etxtEmail.getText().toString().isEmpty() && !etxtLocation.getText().toString().isEmpty() &&
                !etxtVehicleMake.toString().isEmpty() && !etxtVehicleModel.toString().isEmpty()) {
            createUser(new User(etxtUsername.getText().toString(), etxtPassword.getText().toString(),
                    etxtEmail.getText().toString(), etxtLocation.getText().toString(), etxtVehicleMake.getText().toString(),
                    etxtVehicleModel.getText().toString()));
        } else showToast(context.getString(R.string.register_empty_fields));
    }

    @AfterViews
    void init() {
        context = getApplicationContext();
    }

    @Background
    void createUser(User user) {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            Call<APIStatus> createUserCall = service.register(user);
            Response<APIStatus> response = createUserCall.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        loginPrefs.edit().userName().put(user.getUserName()).apply();
                        loginPrefs.edit().password().put(user.getPassword()).apply();
                        loginPrefs.edit().userMail().put(user.getMail()).apply();
                        MainActivity_.intent(this).start();
                        break;
                    case 409:
                        APIStatus status;
                        Converter<ResponseBody, APIStatus> errorConverter = retrofitClient.getRetrofit().responseConverter(APIStatus.class, new Annotation[0]);
                        status = errorConverter.convert(response.errorBody());
                        if (status.getShowUser()) showToast(status.getMessage());
                        else showToast(context.getString(R.string.register_error));
                        break;
                    default:
                        showToast(context.getString(R.string.register_error));
                        break;
                }
            } else throw new IOException("Failed to register user.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("register_error", e.getMessage());
            showToast(context.getString(R.string.register_error));
        }
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

}
