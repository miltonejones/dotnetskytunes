#!/bin/bash

# 1. Create the required IAM role
echo "Creating App Runner service role..."
aws iam create-service-linked-role --aws-service-name apprunner.amazonaws.com

# 2. Wait for role creation
sleep 10

# 3. Create simple config without AccessRoleArn
cat > apprunner-config-simple.json << 'EOF'
{
  "ServiceName": "skytunes-app",
  "SourceConfiguration": {
    "ImageRepository": {
      "ImageIdentifier": "123823813021.dkr.ecr.us-east-1.amazonaws.com/skytunescsharp:latest",
      "ImageRepositoryType": "ECR",
      "ImageConfiguration": {
        "Port": "80",
        "RuntimeEnvironmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Production"
        }
      }
    }
  },
  "InstanceConfiguration": {
    "Cpu": "1024",
    "Memory": "2048"
  }
}
EOF

# 4. Delete the failed service
SERVICE_ARN=$(aws apprunner list-services --query "ServiceSummaryList[?ServiceName=='skytunes-app'].ServiceArn" --output text)
if [ ! -z "$SERVICE_ARN" ]; then
    echo "Deleting failed service..."
    aws apprunner delete-service --service-arn $SERVICE_ARN
    sleep 30
fi

# 5. Create new service
echo "Creating new service with automatic role creation..."
aws apprunner create-service --cli-input-json file://apprunner-config-simple.json

echo "Deployment started! This should work now."


