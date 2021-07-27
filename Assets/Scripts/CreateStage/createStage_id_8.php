<?php

const MIN_STAGE_NO = 106;
const MAX_STAGE_NO = 110;
const NEW_LINE_1 = 5;
const NEW_LINE_2 = 11;
const NEW_LINE_3 = 17;
const NEW_LINE_4 = 23;
const LINE_END = 29;
const MIN_FRUITS_NO = 0;
const MAX_FRUITS_NO = 3;


for($i = MIN_STAGE_NO; $i <= MAX_STAGE_NO; $i++){

    $squaresString = "";
    $count = 0;
    
    while(true){
        if(NEW_LINE_1 == $count || NEW_LINE_2 == $count || NEW_LINE_3 == $count || NEW_LINE_4 == $count){ 
            $squaresString[$count] = "\n";
            $count++;
        }
        if(LINE_END <= $count){
            $count++;
            break;
        }
        $squaresString[$count] = (string) mt_rand(MIN_FRUITS_NO, MAX_FRUITS_NO);
        $count++;
    }

    file_put_contents('../Resorces/Stages/'.$i.".txt", $squaresString);  
    //file_put_contents('../../Assets/Resouces/Stages/'.$i.".txt", $squaresString);  

}

?>