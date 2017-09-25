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
            UNITY_PATH = "BBBBBBBBBBBB"            
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
        UNITY_PATH = "BBBBBB"
        echo "${UNITY_PATH}"
      }
    }
  }
}
