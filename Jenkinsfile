pipeline {
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "error": {
            echo 'hello world'
            
          },
          "": {
            echo 'go'
            
          }
        )
      }
    }
    stage('End') {
      steps {
        echo 'end'
      }
    }
  }
}