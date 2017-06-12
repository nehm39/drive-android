package com.drive.app.model;

import java.util.List;

/**
 * Created by Szymon Gajewski on 19.01.2016.
 */
public class NewsList {
    private List<News> news;
    private List<User> users;
    private List<Event> events;

    public int size() {
        return news.size() + users.size() + events.size();
    }

    public List<News> getNews() {
        return news;
    }

    public List<User> getUsers() {
        return users;
    }

    public List<Event> getEvents() {
        return events;
    }
}
