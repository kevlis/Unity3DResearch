pipeline {
    environment {
        AAAAA = "AAAAAAAAAAAA";
    }
    define {
       def UNITY_PATH = "AAAAAAAAAAAA";
     }
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            script {
              //If def can be anywhere in pipeline {}, drop script {}
              UNITY_PATH = "BBBBBBBBBBBB"  
            }
                      
          },
          "DEF": {
            echo 'go'                
          }
        )
      }
    }
    stage('End') {
      steps {
        echo 'end'
        echo "${UNITY_PATH}"
      }
    }
  }
}
