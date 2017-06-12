package com.drive.app.fragments;


import android.app.Fragment;
import android.content.Context;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.NewsList;
import com.drive.app.other.NewsListAdapter;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;

import java.io.IOException;

import retrofit.Call;
import retrofit.Response;

@EFragment(R.layout.fragment_newsfeed)
public class NewsfeedFragment extends Fragment {

    @Bean
    RetrofitClient retrofitClient;
    @ViewById(R.id.news_recycler_view)
    RecyclerView recyclerView;
    @ViewById(R.id.news_main_layout)
    RelativeLayout mainLayout;
    @ViewById(R.id.news_empty_layout)
    RelativeLayout emptyLayout;
    @ViewById(R.id.news_progress_bar)
    ProgressBar progressBar;

    private Context context;

    public NewsfeedFragment() {
    }

    @AfterViews
    void init() {
        context = getActivity().getApplicationContext();
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        getNews();
    }

    @Background
    void getNews() {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            Call<NewsList> getNews = service.getNews();
            Response<NewsList> response = getNews.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        NewsList news = response.body();
                        setAdapter(news);
                        break;
                    default:
                        showToast(context.getString(R.string.general_error_with_retry));
                        break;
                }
            } else throw new IOException("Failed to get news.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("get_news_error", e.getMessage());
            hideProgressBar();
            showToast(context.getString(R.string.general_error_with_retry));
        }
    }

    @UiThread
    void setAdapter(NewsList newsList) {
        progressBar.setVisibility(View.INVISIBLE);
        if (newsList.size() > 0) {
            NewsListAdapter newsListAdapter = new NewsListAdapter(getActivity(), newsList);
            recyclerView.setAdapter(newsListAdapter);
            emptyLayout.setVisibility(View.GONE);
            mainLayout.setVisibility(View.VISIBLE);
        } else {
            mainLayout.setVisibility(View.GONE);
            emptyLayout.setVisibility(View.VISIBLE);
        }
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

    @UiThread
    void hideProgressBar() {
        progressBar.setVisibility(View.INVISIBLE);
    }

}