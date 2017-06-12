package com.drive.app.fragments;


import android.os.Bundle;
import android.preference.PreferenceFragment;

import com.drive.app.R;

/**
 * Created by Szymon Gajewski on 20.12.2015.
 */
public class SettingsFragment extends PreferenceFragment {

    public SettingsFragment() {
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        addPreferencesFromResource(R.xml.settings);

        //TODO: after "share location" option change, check if permissions are granted, if not: show info dialog and ask for it
    }

}
