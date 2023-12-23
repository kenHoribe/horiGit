package com.websarva.wings.android.asyncsample;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 『Androidアプリ開発の教科書』
 * 第11章
 * Web API連携サンプル
 *
 * アクティビティクラス。
 *
 * @author Shinzo SAITO
 */
public class WeatherInfoActivity extends AppCompatActivity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_weather_info);

		//画面部品ListViewを取得
		ListView lvCityList = findViewById(R.id.lvCityList);
		//SimpleAdapterで使用するListオブジェクトを用意。
		List<Map<String, String>> cityList = new ArrayList<>();
		//都市データを格納するMapオブジェクトの用意とcityListへのデータ登録。
		Map<String, String> city = new HashMap<>();
		city.put("name", "大阪");
		city.put("id", "270000");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "神戸");
		city.put("id", "280010");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "豊岡");
		city.put("id", "280020");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "京都");
		city.put("id", "260010");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "舞鶴");
		city.put("id", "260020");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "奈良");
		city.put("id", "290010");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "風屋");
		city.put("id", "290020");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "和歌山");
		city.put("id", "300010");
		cityList.add(city);
		city = new HashMap<>();
		city.put("name", "潮岬");
		city.put("id", "300020");
		cityList.add(city);
		//SimpleAdapterで使用するfrom-to用変数の用意。
		String[] from = {"name"};
		int[] to = {android.R.id.text1};
		//SimpleAdapterを生成。
		SimpleAdapter adapter = new SimpleAdapter(WeatherInfoActivity.this, cityList, android.R.layout.simple_expandable_list_item_1, from, to);
		//ListViewにSimpleAdapterを設定。
		lvCityList.setAdapter(adapter);
		//ListViewにリスナを設定。
		lvCityList.setOnItemClickListener(new ListItemClickListener());
	}

	/**
	 * リストが選択されたときの処理が記述されたメンバクラス。
	 */
	private class ListItemClickListener implements AdapterView.OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			//ListViewでタップされた行の都市名と都市IDを取得
			Map<String, String> item = (Map<String, String>) parent.getItemAtPosition(position);
			String cityName = item.get("name");
			String cityId = item.get("id");
			//取得した都市名をtvCityNameに設定。
			TextView tvCityName = findViewById(R.id.tvCityName);
			tvCityName.setText(cityName + "の天気: ");
			//天気情報を表示するTextViewを取得。
			TextView tvWeatherTelop = findViewById(R.id.tvWeatherTelop);
			//天気詳細情報を表示するTextViewを取得。
			TextView tvWeatherDesc = findViewById(R.id.tvWeatherDesc);
			//WeatherInfoReceiverをnew。引数として上で取得したTextViewを渡す。
			WeatherInfoReceiver receiver = new WeatherInfoReceiver(tvWeatherTelop, tvWeatherDesc);
			//WeatherInfoReceiverを実行。
			receiver.execute(cityId);
		}
	}

	/**
	 * 非同期でお天気データを取得するクラス。
	 */
	private class WeatherInfoReceiver extends AsyncTask<String, String, String> {
		/**
		 * 現在の天気を表示する画面部品フィールド。
		 */
		private TextView _tvWeatherTelop;
		/**
		 * 天気の詳細を表示する画面部品フィールド。
		 */
		private TextView _tvWeatherDesc;

		/**
		 * コンストラクタ。
		 * お天気情報を表示する画面部品をあらかじめ取得してフィールドに格納している。
		 */
		public WeatherInfoReceiver(TextView tvWeatherTelop, TextView tvWeatherDesc) {
			//引数をそれぞれのフィールドに格納。
			_tvWeatherTelop = tvWeatherTelop;
			_tvWeatherDesc = tvWeatherDesc;
		}

		@Override
		public String doInBackground(String... params) {
			//可変長引数の1個目(インデックス0)を取得。これが都市ID
			String id = params[0];
			//都市IDを使って接続URL文字列を作成。
			String urlStr = "http://weather.livedoor.com/forecast/webservice/json/v1?city=" + id;
			//天気情報サービスから取得したJSON文字列。天気情報が格納されている。
			String result = "";

			//http接続を行うHttpURLConnectionオブジェクトを宣言。finallyで確実に解放するためにtry外で宣言。
			HttpURLConnection con = null;
			//http接続のレスポンスデータとして取得するInputStreamオブジェクトを宣言。同じくtry外で宣言。
			InputStream is = null;
			try {
				//URLオブジェクトを生成。
				URL url = new URL(urlStr);
				//URLオブジェクトからHttpURLConnectionオブジェクトを取得。
				con = (HttpURLConnection) url.openConnection();
				//http接続メソッドを設定。
				con.setRequestMethod("GET");
				//接続。
				con.connect();
				//HttpURLConnectionオブジェクトからレスポンスデータを取得。
				is = con.getInputStream();
				//レスポンスデータであるInputStreamオブジェクトを文字列に変換。
				result = is2String(is);
			}
			catch(MalformedURLException ex) {
			}
			catch(IOException ex) {
			}
			finally {
				//HttpURLConnectionオブジェクトがnullでないなら解放。
				if(con != null) {
					con.disconnect();
				}
				//InputStreamオブジェクトがnullでないなら解放。
				if(is != null) {
					try {
						is.close();
					}
					catch(IOException ex) {
					}
				}
			}

			//JSON文字列を返す。
			return result;
		}

		@Override
		public void onPostExecute(String result) {
			//天気情報用文字列変数を用意。
			String telop = "";
			String desc = "";

			try {
				//JSON文字列からJSONObjectオブジェクトを生成。これをルートJSONオブジェクトとする。
				JSONObject rootJSON = new JSONObject(result);
				//ルートJSON直下の「description」JSONオブジェクトを取得。
				JSONObject descriptionJSON = rootJSON.getJSONObject("description");
				//「description」プロパティ直下の「text」文字列(天気概況文)を取得。
				desc = descriptionJSON.getString("text");
				//ルートJSON直下の「forecasts」JSON配列を取得。
				JSONArray forecasts = rootJSON.getJSONArray("forecasts");
				//「forecasts」JSON配列のひとつ目(インデックス0)のJSONオブジェクトを取得。
				JSONObject forecastNow = forecasts.getJSONObject(0);
				//「forecasts」ひとつ目のJSONオブジェクトから「telop」文字列(天気)を取得。
				telop = forecastNow.getString("telop");
			}
			catch(JSONException ex) {
			}

			//天気情報用文字列をTextViewにセット。
			_tvWeatherTelop.setText(telop);
			_tvWeatherDesc.setText(desc);
		}

		/**
		 * InputStreamオブジェクトを文字列に変換するメソッド。変換文字コードはUTF-8。
		 *
		 * @param is 変換対象のInputStreamオブジェクト。
		 * @return 変換された文字列。
		 * @throws IOException 変換に失敗した時に発生。
		 */
		private String is2String(InputStream is) throws IOException {
			BufferedReader reader = new BufferedReader(new InputStreamReader(is, "UTF-8"));
			StringBuffer sb = new StringBuffer();
			char[] b = new char[1024];
			int line;
			while(0 <= (line = reader.read(b))) {
				sb.append(b, 0, line);
			}
			return sb.toString();
		}
	}
}
