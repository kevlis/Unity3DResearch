pipeline {

  agent none
  stages {
    stage('error') {
      steps {
            parallel(
              "ABC": {
                echo 'hello world'
                  script {
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
