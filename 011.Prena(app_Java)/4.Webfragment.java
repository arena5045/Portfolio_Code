package com.example.Prena;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import androidx.fragment.app.Fragment;

public class Webfragment extends Fragment {

    WebView web;


    public static Webfragment newstance(){
        return new Webfragment();
    }


    @Override
    public View onCreateView(LayoutInflater inflater,ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_web,container,false);



        web = (WebView)view.findViewById(R.id.web);
        web.setWebViewClient(new WebViewClient());
        WebSettings webSettings = web.getSettings(); //웹 세팅
        webSettings.setBuiltInZoomControls(true); // 화면 줌컨트롤 기능
        webSettings.setSupportZoom(true); //줌 서포트가능
        webSettings.setDisplayZoomControls(true);

        web.getSettings().setLoadWithOverviewMode(true);
        web.getSettings().setUseWideViewPort(true);


        web.scrollTo(0,1200);
        web.loadUrl("https://www.bandaimall.co.kr/premium/index.do");


        return  view;
    }

}
