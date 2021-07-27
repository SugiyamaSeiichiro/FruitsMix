<?php

const MIN_STAGE_NO = 1;
const MAX_STAGE_NO = 15;
const NEW_LINE_1 = 3;
const NEW_LINE_2 = 7;
const LINE_END = 11;
const MIN_FRUITS_NO = 0;
const MAX_FRUITS_NO = 2;


for($i = MIN_STAGE_NO; $i <= MAX_STAGE_NO; $i++){

    $squaresString = "";
    $count = 0;
    
    while(true){
        if(NEW_LINE_1 == $count || NEW_LINE_2 == $count){ 
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