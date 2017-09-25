pipeline {
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            
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
      }
    }
  }
}
