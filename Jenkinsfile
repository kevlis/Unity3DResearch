pipeline {
    environment {
       UNITY_PATH = "AAAAAAAAAAAA"
     }
  agent none
  stages {
    stage('error') {
      steps {
          script {
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
    }
    stage('End') {
      steps {
        echo 'end'
        echo "${UNITY_PATH}"
      }
    }
  }
}
