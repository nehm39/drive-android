package com.drive.app.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by Szymon Gajewski on 13.12.2015.
 */
public class APIStatus {
    @SerializedName("showUser")
    private Boolean showUser;
    @SerializedName("message")
    private String message;

    public Boolean getShowUser() {
        return showUser;
    }

    public String getMessage() {
        return message;
    }
}
