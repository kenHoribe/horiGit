package to.msn.wings.tab;

        import android.support.design.widget.TabLayout;
        import android.support.v4.view.ViewPager;
        import android.support.v7.app.AppCompatActivity;
        import android.os.Bundle;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ViewPager pager = findViewById(R.id.pager);
        pager.setAdapter(
                new PageAdapter(getSupportFragmentManager()));

        TabLayout tabs = findViewById(R.id.tabs);
        tabs.setupWithViewPager(pager);
        tabs.getTabAt(0).setIcon(R.drawable.tab_icon1);
        tabs.getTabAt(1).setIcon(R.drawable.tab_icon2);
        tabs.getTabAt(2).setIcon(R.drawable.tab_icon3);
    }
}
