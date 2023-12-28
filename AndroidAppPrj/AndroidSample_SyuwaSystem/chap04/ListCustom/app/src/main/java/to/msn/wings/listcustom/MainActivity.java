package to.msn.wings.listcustom;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ListView;
import android.widget.SimpleAdapter;

import java.util.ArrayList;
import java.util.HashMap;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String titles[] = { "革命のエチュード", "G線上のアリア", "シャコンヌ", "夜の女王のアリア", "春の海" };
        String tags[] = { "ピアノ", "バイオリン", "チェロ", "声楽", "箏" };
        String descs[] = { "ピアノの詩人と言われたショパンの代表的なピアノ曲です。",
                "バッハの作品。バイオリンのG線のみで演奏できることからこのタイトルで親しまれています。",
                "バッハの作品。パルティータ第2番の終曲です。", "モーツァルト作曲のオペラ「魔笛」の中のアリアです。",
                "宮城道雄の作品です。曲の舞台は鞆の浦と言われています。" };
        ArrayList<HashMap<String, String>> data = new ArrayList<>();
        for (int i = 0; i < titles.length; i++) {
            HashMap<String, String> item = new HashMap<>();
            item.put("title", titles[i]);
            item.put("tag", tags[i]);
            item.put("desc", descs[i]);
            data.add(item);
        }
        SimpleAdapter adapter = new SimpleAdapter(this, data, R.layout.list_item,
                new String[] { "title", "tag", "desc" },
                new int[] { R.id.title, R.id.tag, R.id.desc }
        );
        ListView list = findViewById(R.id.list);
        list.setAdapter(adapter);

    }
}
