pipeline {
  agent any
  stages {
    stage('') {
      steps {
        parallel(
          "Hello": {
            echo 'hello world'
            
          },
          "GO": {
            sh 'go'
            
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