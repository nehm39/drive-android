package com.drive.app.activities;

import android.app.ProgressDialog;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.EditText;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.User;
import com.drive.app.prefs.LoginPrefs_;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import org.androidannotations.annotations.sharedpreferences.Pref;

import java.io.IOException;

import retrofit.Call;
import retrofit.Response;

@EActivity(R.layout.activity_login)
public class LoginActivity extends AppCompatActivity {
    @Pref
    LoginPrefs_ loginPrefs;
    @Bean
    RetrofitClient retrofitClient;

    @ViewById(R.id.etxt_username)
    EditText etxtUsername;
    @ViewById(R.id.etxt_password)
    EditText etxtPassword;

    private ProgressDialog loggingProgessDialog;

    private Context context;

    @Click(R.id.login_button)
    void loginButtonClicked() {
        if (!etxtPassword.getText().toString().isEmpty() && !etxtUsername.getText().toString().isEmpty()) {
            loginPrefs.edit().userName().put(etxtUsername.getText().toString()).apply();
            loginPrefs.edit().password().put(etxtPassword.getText().toString()).apply();
            loggingProgessDialog = ProgressDialog.show(this, "", getString(R.string.logging_in), true);
            authenticateUser();
        } else showToast(context.getString(R.string.login_empty_fields));
    }

    @Click(R.id.register_button)
    void registerButtonClicked() {
        RegisterActivity_.intent(this).start();
    }

    @AfterViews
    void init() {
        context = getApplicationContext();
        if (isNetworkAvailable()) {
            if (loginPrefs.userName().exists() && loginPrefs.password().exists()) {
                loggingProgessDialog = ProgressDialog.show(this, "", getString(R.string.logging_in), true);
                authenticateUser();
            }
        } else {
            showToast(getString(R.string.no_internet));
        }
    }

    private boolean isNetworkAvailable() {
        ConnectivityManager connectivityManager
                = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnected();
    }

    @Background
    void authenticateUser() {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            Call<User> authenticateCall = service.authenticate();
            Response<User> response = authenticateCall.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        loginPrefs.edit().userMail().put(response.body().getMail()).apply();
                        MainActivity_.intent(this).start();
                        break;
                    case 403:
                        showToast(context.getString(R.string.wrong_credentials));
                        loginPrefs.clear();
                        break;
                    default:
                        showToast(context.getString(R.string.login_error));
                        loginPrefs.clear();
                        break;
                }
                hideProgressDialog();
            } else throw new IOException("Failed to authenticate.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("authenticate_error", e.getMessage());
            showToast(context.getString(R.string.login_error));
            hideProgressDialog();
        }
    }

    @UiThread
    void hideProgressDialog() {
        loggingProgessDialog.dismiss();
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }
}
