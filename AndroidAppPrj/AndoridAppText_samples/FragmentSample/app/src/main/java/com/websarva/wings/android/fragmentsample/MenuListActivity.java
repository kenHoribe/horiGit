package com.websarva.wings.android.fragmentsample;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

/**
 * 『Androidアプリ開発の教科書』
 * 第9章
 * フラグメントサンプル
 *
 * 注文リスト画面のアクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class MenuListActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_menu_list);
	}
}
