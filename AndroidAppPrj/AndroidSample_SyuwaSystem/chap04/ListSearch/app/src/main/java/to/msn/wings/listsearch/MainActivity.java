package to.msn.wings.listsearch;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.SearchView;

import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ArrayList<String> data = new ArrayList<>();
        data.add("胡椒");
        data.add("ターメリック");
        data.add("コリアンダー");
        data.add("生姜");
        data.add("ニンニク");
        data.add("サフラン");
        data.add("オレガノ");
        data.add("クミン");
        data.add("クローブ");
        data.add("シナモン");
        data.add("ナツメグ");
        data.add("セージ");
        data.add("タイム");
        data.add("パクチー");

        ArrayAdapter<String> adapter = new ArrayAdapter<>(
            this, android.R.layout.simple_list_item_1, data);
        final ListView list = findViewById(R.id.list);
        list.setAdapter(adapter);
        list.setTextFilterEnabled(true);

        SearchView sv = findViewById(R.id.search);
        sv.setOnQueryTextListener(
            new SearchView.OnQueryTextListener() {
                public boolean onQueryTextChange(String text) {
                    if (text == null || text.equals("")) {
                        list.clearTextFilter();
                    } else {
                        list.setFilterText(text);
                    }
                    return false;
                }

                public boolean onQueryTextSubmit(String arg0) {
                    return false;
                }
            }
        );
    }
}
