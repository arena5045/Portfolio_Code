package com.example.Prena;

import android.content.Intent;
import android.content.res.TypedArray;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

public class GundamList extends Fragment {

    String[] name;//이름값 배열


    public static GundamList newstance(){
        return new GundamList();
    }

    GundamListAdaptar adapter;
    ListView listView;


    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {


        final Intent intent = new Intent( getContext(), GundamInfo.class);

        View view  = inflater.inflate(R.layout.fragment_gundamlist,container,false);

        adapter = new GundamListAdaptar();
        listView = (ListView) view.findViewById(R.id.listView);


            if(MainActivity.DoSearch==true) // 두서치 불값이 트루면
            {
                if( MainActivity.SearchName != null) {//그리고 이름서치값이 트루면
                    NameSearchData(MainActivity.SearchName); //이름서치데이터 메서드 실행
                }else if(MainActivity.SearchScale != null)// 스케일 서치값이 트루면
                {
                    ScaleSearchData(MainActivity.SearchScale);//스케일서치 메서드 실행
                }

                MainActivity.DoSearch=false;//다른 불값 초기화
                MainActivity.SearchName=null;
                MainActivity.SearchScale=null;
            }else{//검색으로 들어온게 아니면
                setData();//모든 데이터 검색
            }


        listView.setAdapter(adapter);//리스트 뷰 어댑터 등록


        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                TextView nametxt = view.findViewById(R.id.name);
                String gundamname = nametxt.getText().toString();
               intent.putExtra("gundamname", gundamname );//해당 건담의 이름 인텐트로 보냄

               startActivity(intent);
            }
        });//리스트뷰의 클릭리스너 추가

        return view; //뷰를 리턴
    }


    private void setData() {//데이터 설정 메서드

        TypedArray arrResId = getResources().obtainTypedArray(R.array.resId); //drawvle은 타입드 어레이에 저장해야한다.
        String[] titles = getResources().getStringArray(R.array.title);
        String[] contents = getResources().getStringArray(R.array.content);
        String[] scale = getResources().getStringArray(R.array.scale);
        name = new String[arrResId.length()];//이름값을 보내주기 위해 따로 저장


        for (int i = 0; i < arrResId.length(); i++) {//순서대로 어댑터에 아이템 추가
            customDTO dto = new customDTO();
            dto.setResId(arrResId.getResourceId(i, 0));
            dto.setTitle(titles[i]);
            dto.setContent(contents[i]);
            dto.setScale(scale[i]);
            name[i] = titles[i];

            adapter.addItem(dto);
        }

    }


    private void NameSearchData(String sname) {//이름검색

        TypedArray arrResId = getResources().obtainTypedArray(R.array.resId); //drawvle은 타입드 어레이에 저장해야한다.
        String[] titles = getResources().getStringArray(R.array.title);
        String[] contents = getResources().getStringArray(R.array.content);
        String[] scale = getResources().getStringArray(R.array.scale);
        name = new String[arrResId.length()];


        for (int i = 0; i < arrResId.length(); i++) {

            if (titles[i].contains(sname)) {//모든검색에서 이름이랑 검색조건이랑 같을경우를 추가
                customDTO dto = new customDTO();
                dto.setResId(arrResId.getResourceId(i, 0));
                dto.setTitle(titles[i]);
                dto.setContent(contents[i]);
                dto.setScale(scale[i]);
                name[i] = titles[i];

                adapter.addItem(dto);
            }
        }
    }

    private void ScaleSearchData(String Sscale) {//스케일 검색

        TypedArray arrResId = getResources().obtainTypedArray(R.array.resId); //drawvle은 타입드 어레이에 저장해야한다.
        String[] titles = getResources().getStringArray(R.array.title);
        String[] contents = getResources().getStringArray(R.array.content);
        String[] scale = getResources().getStringArray(R.array.scale);
        name = new String[arrResId.length()];


        for (int i = 0; i < arrResId.length(); i++) {

            if (scale[i].contains(Sscale)) {//모든검색에서 등급이랑 등급조건이랑 같을경우를 추가
                customDTO dto = new customDTO();
                dto.setResId(arrResId.getResourceId(i, 0));
                dto.setTitle(titles[i]);
                dto.setContent(contents[i]);
                dto.setScale(scale[i]);
                name[i] = titles[i];

                adapter.addItem(dto);
            }
        }
    }

}