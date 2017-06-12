package com.drive.app.model;

/**
 * Created by Szymon Gajewski on 18.01.2016.
 */
public class UsersNearbyFilter {

    private int maxDistance;
    private double latitude;
    private double longitude;

    public UsersNearbyFilter(int maxDistance, double latitude, double longitude) {
        this.maxDistance = maxDistance;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public int getMaxDistance() {
        return maxDistance;
    }

    public void setMaxDistance(int maxDistance) {
        this.maxDistance = maxDistance;
    }

    public double getLatitude() {
        return latitude;
    }

    public void setLatitude(double latitude) {
        this.latitude = latitude;
    }

    public double getLongitude() {
        return longitude;
    }

    public void setLongitude(double longitude) {
        this.longitude = longitude;
    }
}
