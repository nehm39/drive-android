package com.drive.app.other;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.drive.app.R;
import com.drive.app.model.Event;
import com.drive.app.model.News;
import com.drive.app.model.NewsList;
import com.drive.app.model.User;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Szymon Gajewski on 19.01.2016.
 */
public class NewsListAdapter extends RecyclerView.Adapter<NewsHolder> {

    private final List<String> newsList;

    public NewsListAdapter(Context context, NewsList news) {
        newsList = new ArrayList<>();

        for (News n : news.getNews()) {
            newsList.add(n.getContent());
        }

        for (Event e : news.getEvents()) {
            newsList.add(context.getString(R.string.events_nearby_news) + " " + e.getName() + ".");
        }

        for (User u : news.getUsers()) {
            newsList.add(context.getString(R.string.users_nearby_news) + " " + u.getUserName() + ".");
        }
    }


    @Override
    public NewsHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.news_row, parent, false);
        return new NewsHolder(view);
    }

    @Override
    public void onBindViewHolder(NewsHolder holder, int position) {
        String news = newsList.get(position);
        holder.txtNewsContent.setText(news);
    }

    @Override
    public int getItemCount() {
        return newsList.size();
    }
}
