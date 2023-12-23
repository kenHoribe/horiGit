package com.websarva.wings.android.listviewsample;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

/**
 * 『Androidアプリ開発の教科書』
 * 第5章
 * リスト選択サンプル
 *
 * メインアクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class ListViewSampleActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_list_view_sample);

		//ListViewオブジェクトを取得。
		ListView lvMenu = findViewById(R.id.lvMenu);
		//ListViewにリスナを設定。
		lvMenu.setOnItemClickListener(new ListItemClickListener());
	}

	/**
	 * リストがタップされたときの処理が記述されたメンバクラス。
	 */
	private class ListItemClickListener implements AdapterView.OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			//タップされた定食名を取得。
			String item = (String) parent.getItemAtPosition(position);
			//トーストで表示する文字列を生成。
			String show = "あなたが選んだ定食: " + item;
			//トーストの表示。
			Toast.makeText(ListViewSampleActivity.this, show, Toast.LENGTH_LONG).show();
		}
	}
}
