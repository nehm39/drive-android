package com.drive.app.rest;

import com.drive.app.model.APIStatus;
import com.drive.app.model.Event;
import com.drive.app.model.EventsNearbyFilter;
import com.drive.app.model.GeoPoint;
import com.drive.app.model.NewsList;
import com.drive.app.model.User;
import com.drive.app.model.UsersNearbyFilter;
import com.drive.app.prefs.LoginPrefs_;
import com.google.gson.Gson;
import com.squareup.okhttp.Interceptor;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.Response;
import com.squareup.okhttp.logging.HttpLoggingInterceptor;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EBean;
import org.androidannotations.annotations.sharedpreferences.Pref;

import java.io.IOException;
import java.util.List;

import retrofit.Call;
import retrofit.GsonConverterFactory;
import retrofit.Retrofit;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.PUT;
import retrofit.http.Path;

/**
 * Created by Szymon Gajewski on 12.12.2015.
 */
@EBean(scope = EBean.Scope.Singleton)
public class RetrofitClient {
    private static final String API_URL = "http://drivingapptmp-001-site1.anytempurl.com/Service.svc/";
    public static final String API_EVENTS_IMAGES_URL = "http://drivingapptmp-001-site1.anytempurl.com/events_images/";
    private ApiService apiService;
    private Retrofit retrofit;

    public ApiService getApiService() {
        return apiService;
    }

    public Retrofit getRetrofit() {
        return retrofit;
    }

    @Pref
    LoginPrefs_ loginPrefs;

    @AfterInject
    protected void start() {
        Gson gson = new Gson();

        OkHttpClient okHttpClient = new OkHttpClient();
        HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
        interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);
        okHttpClient.interceptors().add(interceptor);
        okHttpClient.networkInterceptors().add(new Interceptor() {
            @Override
            public Response intercept(Chain chain) throws IOException {
                Request request = chain.request().newBuilder().addHeader("Username", loginPrefs.userName().getOr("")).addHeader("Password", loginPrefs.password().getOr("")).build();
                return chain.proceed(request);
            }
        });

        retrofit = new Retrofit.Builder()
                .baseUrl(API_URL)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .client(okHttpClient)
                .build();

        apiService = retrofit.create(ApiService.class);
    }

    public interface ApiService {
        @GET("authenticate")
        Call<User> authenticate();

        @GET("user")
        Call<User> getUserForProfile();

        @PUT("user/location")
        Call<APIStatus> updateUserLocation(@Body GeoPoint geoPoint);

        @POST("user")
        Call<APIStatus> register(@Body User user);

        @PUT("user")
        Call<APIStatus> updateUser(@Body User user);

        @POST("users/nearby")
        Call<List<User>> getUsersNearby(@Body UsersNearbyFilter filter);

        @POST("events/nearby")
        Call<List<Event>> getEventsNearby(@Body EventsNearbyFilter filter);

        @GET("events")
        Call<List<Event>> getAllEvents();

        @GET("users/search/{username}")
        Call<List<User>> getUsersByName(@Path("username") String username);

        @GET("news")
        Call<NewsList> getNews();

    }

    public RetrofitClient() {
    }
}
