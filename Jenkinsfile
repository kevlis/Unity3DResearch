pipeline {
    environment {
        UNITY_PATH = "AAAAAAAAAAAA";
    }

  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            AAAA = "BBBBBBBBBBBB"            
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
        AAAA = "BBBBBB"
        echo "${AAAA}"
      }
    }
  }
}
