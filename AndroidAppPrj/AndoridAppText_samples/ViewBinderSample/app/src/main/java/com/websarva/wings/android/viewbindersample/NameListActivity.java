package com.websarva.wings.android.viewbindersample;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 『Androidアプリ開発の教科書』
 * 第7章 Note
 * ViewBinderサンプル
 *
 * 名前リスト画面のアクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class NameListActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_name_list);

		//SimpleAdapterで使用する名前Listオブジェクトをprivateメソッドを利用して用意。
		List<Map<String, Object>> nameList = createNameList();
		//SimpleAdapterの第4引数fromに使用する変数を用意。
		String[] from = {"name", "sex"};
		//SimpleAdapterの第5引数toに使用する変数を用意。
		int[] to = {R.id.tvName, R.id.imSex};
		//SimpleAdapterを生成。
		SimpleAdapter adapter = new SimpleAdapter(NameListActivity.this, nameList, R.layout.row, from, to);
		//カスタムビューバインダを登録。
		adapter.setViewBinder(new CustomViewBinder());
		//画面部品ListViewを取得。
		ListView lvPhones = findViewById(R.id.lvNameList);
		//アダプタの登録。
		lvPhones.setAdapter(adapter);
	}

	/**
	 * リストビューに表示させる名前リストデータを生成するメソッド。
	 *
	 * @return 生成された名前リストデータ。
	 */
	private List<Map<String, Object>> createNameList() {
		//名前リスト用のListオブジェクトを用意。
		List<Map<String, Object>> nameList = new ArrayList<>();

		//一人目のデータを格納するMapオブジェクトの用意とnameListへのデータ登録。
		Map<String, Object> person = new HashMap<>();
		person.put("name", "田中一郎");
		person.put("sex", 1);
		nameList.add(person);

		//二人目のデータを格納するMapオブジェクトの用意とnameListへのデータ登録。
		person = new HashMap<>();
		person.put("name", "江藤香織");
		person.put("sex", 0);
		nameList.add(person);

		//以下データ登録の繰り返し。
		person = new HashMap<>();
		person.put("name", "中山裕子");
		person.put("sex", 0);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "中谷源蔵");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "山下直美");
		person.put("sex", 0);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "鈴木翔太");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "石橋信二");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "杉本孝典");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "牧野知子");
		person.put("sex", 0);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "三上春香");
		person.put("sex", 0);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "大野弘明");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "西口健太");
		person.put("sex", 1);
		nameList.add(person);

		person = new HashMap<>();
		person.put("name", "西野明美");
		person.put("sex", 0);
		nameList.add(person);

		return nameList;
	}

	/**
	 * リストビューのカスタムビューバインダクラス。
	 */
	private class CustomViewBinder implements SimpleAdapter.ViewBinder {

		@Override
		public boolean setViewValue(View view, Object data, String textRepresentation) {
			/*
			 * 引数のviewはリスト1行内でデータを割り当てる画面部品。
			 * dataはそれに割り当てるデータ。
			 * textRepresentationはdataを文字列に変換したデータ。
			 *
			 * viewとdataの組合わせはfromとtoの組合せそのもの。
			 */

			//リスト1行内でデータを割り当てる画面部品のidのR値を取得。
			int viewId = view.getId();
			//idのR値に応じて分岐。
			switch(viewId) {
				//名前を表示するTextViewなら…
				case R.id.tvName:
					//TextViewにキャストして、表示データ(名前)をセットする。
					TextView tvName = (TextView) view;
					tvName.setText(textRepresentation);
					return true;
				//性別アイコンを表示するImageViewなら…
				case R.id.imSex:
					//ImageViewにキャスト。
					ImageView imPhoneType = (ImageView) view;
					//表示データを整数型にキャスト。
					int sex = (Integer) data;
					//表示データに応じて処理を分岐。
					switch(sex) {
						//女性なら…
						case 0:
							//女性アイコンをセット。
							imPhoneType.setImageResource(R.drawable.ic_female);
							break;
						//男性なら…
						case 1:
							//男性アイコンをセット。
							imPhoneType.setImageResource(R.drawable.ic_male);
							break;
					}
					return true;
			}
			return false;
		}
	}
}
