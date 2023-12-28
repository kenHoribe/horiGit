package to.msn.wings.viewpagercustom;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

public class PageAdapter extends FragmentPagerAdapter {
    private final int PAGE_COUNT = 3;
    private String[] titles = new String[] { "ゾウ", "キャラクター", "アザラシ" };

     PageAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public int getCount() {
        return PAGE_COUNT;
    }

    @Override
    public CharSequence getPageTitle(int position) {
        return this.titles[position];
    }

    @Override
    public Fragment getItem(int i) {
        switch(i){
            case 0:
                return new Page1Fragment();
            case 1:
                return new Page2Fragment();
            default:
                return new Page3Fragment();
        }
    }
}
