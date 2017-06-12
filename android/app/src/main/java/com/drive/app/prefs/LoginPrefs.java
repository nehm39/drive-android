package com.drive.app.prefs;

import org.androidannotations.annotations.sharedpreferences.SharedPref;

/**
 * Created by Szymon Gajewski on 13.12.2015.
 */
@SharedPref(SharedPref.Scope.UNIQUE)
public interface LoginPrefs {
    String userName();
    String password();
    String userMail();
}