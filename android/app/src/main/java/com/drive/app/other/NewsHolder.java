package com.drive.app.other;

import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.TextView;

import com.drive.app.R;

/**
 * Created by Szymon Gajewski on 19.01.2016.
 */
public class NewsHolder extends RecyclerView.ViewHolder {

    final TextView txtNewsContent;

    public NewsHolder(View view) {
        super(view);
        this.txtNewsContent = (TextView) view.findViewById(R.id.news_content);
    }
}
