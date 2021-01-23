
	var q1ans;
	var q2ans;
	var q3ans;
	var q4ans;
	var q5ans;


function q1(ans) { //1번질문 응답

	var question = document.getElementById('Q'); 
		var yes = document.getElementById('yes');
		var no = document.getElementById('no');

	if (ans=='yes'){


			q1ans = "오프라인";

			
			
	}
		else{
			q1ans = "인터넷";


			}
			yes.setAttribute("onclick","q2('yes')");
			no.setAttribute("onclick","q2('no')");
			question.innerHTML="질문 2 <br><br> 큰것 보다는 작은 프라모델을 선호하나요?";
}

function q2(ans) { //2번질문 응답

var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		if (ans=='yes'){


			q2ans = "hg";



		}
		else{
			q2ans = "mgpg";
		}		

		yes.setAttribute("onclick","q3('yes')");
		no.setAttribute("onclick","q3('no')");
		question.innerHTML="질문 3 <br><br>심플한게 좋으신가요?";
	}

	function q3(ans) { //3번질문 응답

var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		if (ans=='yes'){

			q3ans = "심플";
		
			if(q1ans=="인터넷" && q2ans=="hg")
			{resultCG1();}
			else if(q1ans=="인터넷" && q2ans=="mgpg"){
				resultCG3();
			}
			else{
				question.innerHTML="질문 4 <br><br>강철 느낌의 코팅은 좋으신가요?";
		yes.setAttribute("onclick","q4('yes')");
		no.setAttribute("onclick","q4('no')");
		}
	}
		else{
			q3ans = "복잡";

			if(q1ans=="인터넷" && q2ans=="hg")
			{resultCG2();}
			else if(q1ans=="인터넷" && q2ans=="mgpg"){
				resultCG4();
			}
			else{
		question.innerHTML="질문 4 <br><br>강철 느낌의 코팅은 좋으신가요?";
		yes.setAttribute("onclick","q4('yes')");
		no.setAttribute("onclick","q4('no')");
		}	
	}	

	}

	function q4(ans) { //4번질문 응답

var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		if (ans=='yes'){

			q4ans = "강철";
			if(q2ans=="hg" && q3ans=="심플")
			{resultOFF1();}
			else if(q2ans=="hg" && q3ans=="복잡"){
				resultOFF2();
			}
			else if(q2ans=="mgpg" && q3ans=="심플"){
				resultOFF3();
			}
			else if(q2ans=="mgpg" && q3ans=="복잡"){
				resultOFF4();
			}
			
		}
		else{

			q4ans = "강철아님";

			if(q2ans=="hg" && q3ans=="심플")
			{resultOFF5();}
			else if(q2ans=="hg" && q3ans=="복잡"){
				resultOFF6();
			}
			else if(q2ans=="mgpg" && q3ans=="심플"){
				resultOFF7();
			}
			else if(q2ans=="mgpg" && q3ans=="복잡"){
				resultOFF8();
			}
			
		}
		

		}		


//------------------------------------------------------------------------------------------------------//
	function resultCG1(){ //인터넷 hg 심플
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*2+1);
	switch(re){
		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/HG/제간 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/HG/짐스나이퍼 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

	function resultCG2(){//인터넷 hg 복잡
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*2+1);
	switch(re){
		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/HG/실버 불릿 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/HG/리바우 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
	yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
}

	function resultCG3(){//인터넷 mgpg 심플
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*4+1);
	switch(re){

		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/MG/크로스본 건담 x2 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/MG/프로토 건담 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 3 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/MG/알트론 건담 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 4 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/PG/건담 어스트레이 블루프레임 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");

	}	
	
	function resultCG4(){//인터넷 mgpg 복잡
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*4+1);
	switch(re){

		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/PG/유니콘 건담 풀아머 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/PG/유니콘 2호기 밴시 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 3 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/PG/유니콘 3호기 페넥스 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 4 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>프리미엄 반다이 한정/MG/건담 헤비암즈 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

//------------------------------------------------------------------------------------------------------//
function resultOFF1(){//오프라인 hg 심플 강철
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*2+1);
	

		question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/스페셜 코팅/나이팅게일bb 스페셜코팅 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";
	
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

	function resultOFF2(){//오프라인 hg 복자 강철
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		 question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/티타늄 피니시/네러티브 건담 c장비 티타늄 피니시 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";
	
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

	function resultOFF3(){//오프라인 mg 심플 강철
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		 question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/스페셜 코팅/사자비 스페셜 코팅 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";
	
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

	function resultOFF4(){//오프라인 mg 복잡 강철
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

		 question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/티타늄 피니시/v2 어설트 버스터 건담 ver.ka 티타늄 피니시 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";
	
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}
//------------------------------------------------------------------------------------------------------//

function resultOFF5(){//오프라인 hg 심플 강철아님
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*5+1);
	switch(re){

		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/메탈릭 글로스 인젝션/퍼스트 건담 메탈릭 글로스 인젝션 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/메탈릭 글로스 인젝션/건담 발바토스 메탈릭 글로스 인젝션 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 3 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2015신작/건담 G-셀프(대기권 팩 장비형)(컬러 클리어) 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 4 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2015신작/G3 건담 (리바이브)입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 5 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2016신작/검은 삼연성 쁘띠 가이입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}


	function resultOFF6(){//오프라인 hg 복잡 강철아님
		var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');

	var re = Math.floor(Math.random()*5+1);
	switch(re){

		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2015 신작/트라이 버닝 건담(pp.클리어 ver) 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2016 신작/크로스 본 건담 x1 풀크로스 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 3 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2016 신작/하이뉴 건담 브레이브 어메이징 ver.붉은 혜성 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 4 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2017 신작/제타 리바이브 클리어컬러 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 5 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건프라 엑스포 한정/2017 신작/스트라이크 프리덤 건담 클리어컬러 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	}
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}


	function resultOFF7(){//오프라인 mgpg 심플 강철아님
	var question = document.getElementById('Q'); 
	var yes = document.getElementById('yes');
	var no = document.getElementById('no');


		 question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/클리어컬러/퍼스트 건담 클리어 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}

	function resultOFF8(){//오프라인 mgpg 복잡 강철아님
		var question = document.getElementById('Q'); 
		var yes = document.getElementById('yes');
		var no = document.getElementById('no');


	var re = Math.floor(Math.random()*2+1);
	switch(re){

		case 1 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/클리어 컬러/프리덤 건담 클리어 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
		case 2 : question.innerHTML="<h1>당신에게 추천하는 한정판 프라모델은 <br> <p style='color: rgb(19, 138, 185);'>건담베이스 한정/클리어 컬러/트랜스 암 라이저 클리어 입니다!</p> 왼쪽 메뉴에서 바로 검색해보세요!</h1>";break;
	
	}
	
	
		yes.setAttribute("onclick","");
		no.setAttribute("onclick","");
	}


	