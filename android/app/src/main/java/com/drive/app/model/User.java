package com.drive.app.model;

/**
 * Created by Szymon Gajewski on 13.12.2015.
 */
public class User {
    public void setId(int id) {
        this.id = id;
    }

    private int id;

    public void setUserName(String userName) {
        this.userName = userName;
    }

    private String userName;
    private String password;
    private String mail;
    private String city;
    private double latitude;
    private double longitude;
    private String vehicleMake;
    private String vehicleModel;
    private String birthdate;

    public String getVehicleMake() {
        return vehicleMake;
    }

    public void setVehicleMake(String vehicleMake) {
        this.vehicleMake = vehicleMake;
    }

    public String getVehicleModel() {
        return vehicleModel;
    }

    public void setVehicleModel(String vehicleModel) {
        this.vehicleModel = vehicleModel;
    }

    public int getId() {
        return id;
    }

    public String getUserName() {
        return userName;
    }

    public String getPassword() {
        return password;
    }

    public String getMail() {
        return mail;
    }

    public String getCity() {
        return city;
    }

    public double getLongitude() {
        return longitude;
    }

    public double getLatitude() {
        return latitude;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public User(int id, String userName, String password, String mail, String city) {
        this.id = id;
        this.userName = userName;
        this.password = password;
        this.mail = mail;
        this.city = city;
    }

    public User(String userName, String password, String mail, String city, String vehicleMake, String vehicleModel) {
        this.userName = userName;
        this.password = password;
        this.mail = mail;
        this.city = city;
        this.vehicleMake = vehicleMake;
        this.vehicleModel = vehicleModel;
        birthdate = "";
    }

    public User() {
        birthdate = "";
    }
}
