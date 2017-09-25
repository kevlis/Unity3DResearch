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
            UNITY_PATH = "BBBBBBBBBBBB" 
            echo "${UNITY_PATH}"
            echo UNITY_PATH
          },
          "DEF": {
            echo 'go'                
            echo "${UNITY_PATH}"
            UNITY_PATH = "CCCCCCCCCC" 
            echo UNITY_PATH
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
