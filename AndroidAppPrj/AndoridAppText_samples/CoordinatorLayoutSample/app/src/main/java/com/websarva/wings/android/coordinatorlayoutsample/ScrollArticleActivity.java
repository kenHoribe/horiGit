package com.websarva.wings.android.coordinatorlayoutsample;

import android.graphics.Color;
import android.support.design.widget.CollapsingToolbarLayout;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;

/**
 * 『Androidアプリ開発の教科書』
 * 第15章
 * ツスロール連動サンプル
 *
 * アクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class ScrollArticleActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_scroll_article);

		//Toolbarを取得。
		Toolbar toolbar = findViewById(R.id.toolbar);
		//ツールバーにロゴを設定。
		toolbar.setLogo(R.mipmap.ic_launcher);
		//アクションバーにツールバーを設定。
		setSupportActionBar(toolbar);
		//CollapsingToolbarLayoutを取得。
		CollapsingToolbarLayout toolbarLayout = findViewById(R.id.toolbarLayout);
		//タイトルを設定。
		toolbarLayout.setTitle(getString(R.string.toolbar_title));
		//通常サイズ時の文字色を設定。
		toolbarLayout.setExpandedTitleColor(Color.WHITE);
		//縮小サイズ時の文字色を設定。
		toolbarLayout.setCollapsedTitleTextColor(Color.LTGRAY);
	}
}
