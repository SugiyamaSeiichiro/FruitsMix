<?php

const MIN_STAGE_NO = 76;
const MAX_STAGE_NO = 90;
const NEW_LINE_1 = 4;
const NEW_LINE_2 = 9;
const NEW_LINE_3 = 14;
const LINE_END = 19;
const MIN_FRUITS_NO = 0;
const MAX_FRUITS_NO = 4;


for($i = MIN_STAGE_NO; $i <= MAX_STAGE_NO; $i++){

    $squaresString = "";
    $count = 0;
    
    while(true){
        if(NEW_LINE_1 == $count || NEW_LINE_2 == $count || NEW_LINE_3 == $count){ 
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