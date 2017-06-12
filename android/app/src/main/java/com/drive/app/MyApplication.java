package com.drive.app;

import android.app.Application;

import net.danlew.android.joda.JodaTimeAndroid;

/**
 * Created by Szymon Gajewski on 11.01.2016.
 */
public class MyApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        JodaTimeAndroid.init(this);
    }
}