pipeline {
    //Use the following docker image to run your dotnet app.
    agent { docker { image 'mcr.microsoft.com/dotnet/core/sdk:2.2-alpine' } }
    environment {HOME = '/tmp'} 
    stages {
        
    // Get some code from a GitHub repository
    stage('Git') {
      steps{
          git 'https://github.com/aladiha/nehool-project.git'
      }
   }
   /* stage('Dotnet Restore'){
        steps{
        sh "dotnet restore"
        }
    }*/
    //
stage('Restore PACKAGES') {
   steps {
    sh "dotnet restore --configfile NuGet.Config"
   }
  }
  stage('Clean') {
   steps {
    sh 'dotnet clean'
   }
  }
  stage('Build') {
   steps {
    sh 'dotnet build --configuration Release'
   }
  }
  stage('Pack') {
   steps {
    sh 'dotnet pack --no-build --output nupkgs'
   }
  }

  }
 }
}
