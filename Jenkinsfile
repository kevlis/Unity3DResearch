pipeline {
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            echo "${UNITY_PATH}"
          },
          "DEF": {
            echo 'go'
            echo "${UNITY_PATH}"
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
