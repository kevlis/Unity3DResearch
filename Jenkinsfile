pipeline {
  agent none
  stages {
    stage('error') {
      steps {
        parallel(
          "ABC": {
            echo 'hello world'
            script {
              def AAV = "AVCD"
              AAV = "ADV"
            }
            
            
          },
          "DEF": {
            echo 'go'
            archiveArtifacts(allowEmptyArchive: true, artifacts: 'package/A/*.apk', onlyIfSuccessful: true, excludes: '*.ipa')
            
          }
        )
      }
    }
    stage('End') {
      steps {
        echo 'end'
        echo '"${AAV}"'
      }
    }
  }
}