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
            env.UNITY_PATH = "BBBBBBBBBBBB"            
          },
          "DEF": {
            echo 'go'                
            env.UNITY_PATH = "CCCCCCCCCC"
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
