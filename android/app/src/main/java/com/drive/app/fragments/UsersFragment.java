package com.drive.app.fragments;


import android.app.Fragment;
import android.app.ProgressDialog;
import android.app.SearchManager;
import android.content.Context;
import android.support.v4.view.MenuItemCompat;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.SearchView;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.widget.Toast;

import com.drive.app.R;
import com.drive.app.model.User;
import com.drive.app.other.UsersListAdapter;
import com.drive.app.rest.RetrofitClient;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;

import java.io.IOException;
import java.util.List;

import retrofit.Call;
import retrofit.Response;

@EFragment(R.layout.fragment_users)
public class UsersFragment extends Fragment {

    @ViewById(R.id.users_recycler_view)
    RecyclerView recyclerView;
    @Bean
    RetrofitClient retrofitClient;
    private Context context;
    private ProgressDialog progressDialog;

    public UsersFragment() {
    }

    @AfterViews
    void init() {
        context = getActivity().getApplicationContext();
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        setHasOptionsMenu(true);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        inflater.inflate(R.menu.search_menu, menu);
        final MenuItem searchMenuItem = menu.findItem(R.id.action_search);
        searchMenuItem.expandActionView();
        final SearchView searchView = (SearchView) MenuItemCompat.getActionView(menu.findItem(R.id.action_search));
        SearchManager searchManager = (SearchManager) getActivity().getSystemService(Context.SEARCH_SERVICE);
        searchView.setSearchableInfo(searchManager.getSearchableInfo(getActivity().getComponentName()));
        searchView.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
            @Override
            public boolean onQueryTextSubmit(String query) {
                showProgressDialog();
                if (!searchView.isIconified()) {
                    searchView.setIconified(true);
                }
                getUsers(query, searchMenuItem);
                return false;
            }

            @Override
            public boolean onQueryTextChange(String s) {
                return false;
            }
        });
        super.onCreateOptionsMenu(menu, inflater);
    }

    @Background
    void getUsers(String query, MenuItem searchMenuItem) {
        try {
            RetrofitClient.ApiService service = retrofitClient.getApiService();
            Call<List<User>> getUsersCall = service.getUsersByName(query);
            Response<List<User>> response = getUsersCall.execute();
            if (response != null) {
                switch (response.code()) {
                    case 200:
                        collapseSearchView(searchMenuItem);
                        setAdapter(response.body());
                        break;
                }
            } else throw new IOException("Failed to get users.");
        } catch (IOException e) {
            if (e.getMessage() != null) Log.e("search_error", e.getMessage());
            showToast(context.getString(R.string.general_error_with_retry));
        }
    }

    @UiThread
    void showProgressDialog() {
        progressDialog = ProgressDialog.show(getActivity(), "", getString(R.string.loading), true);
    }

    @UiThread
    void setAdapter(List<User> users) {
        UsersListAdapter usersListAdapter = new UsersListAdapter(users);
        recyclerView.setAdapter(usersListAdapter);
        progressDialog.dismiss();
    }

    @UiThread
    void collapseSearchView(MenuItem searchMenuItem) {
        searchMenuItem.collapseActionView();
    }

    @UiThread
    void showToast(String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

}
