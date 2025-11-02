#!/bin/bash

SERVICE_ARN=$(aws apprunner list-services --query "ServiceSummaryList[?ServiceName=='skytunes-healthcheck'].ServiceArn" --output text)

echo "Monitoring domain status..."
while true; do
    DOMAIN_INFO=$(aws apprunner describe-custom-domains --service-arn $SERVICE_ARN --query 'CustomDomains[0]' 2>/dev/null)
    
    if [ "$DOMAIN_INFO" != "null" ] && [ ! -z "$DOMAIN_INFO" ]; then
        STATUS=$(echo $DOMAIN_INFO | jq -r '.Status')
        TARGET=$(echo $DOMAIN_INFO | jq -r '.DNSTarget')
        
        echo "$(date): Domain Status = $STATUS"
        
        if [ "$STATUS" = "ACTIVE" ] && [ "$TARGET" != "null" ]; then
            echo "🎉 DOMAIN READY!"
            echo "Add this CNAME record to Route 53:"
            echo "Name: dotnet.skytunes.nl"
            echo "Type: CNAME" 
            echo "Value: $TARGET"
            echo "TTL: 300"
            break
        elif [ "$STATUS" = "PENDING_CERTIFICATE_DNS_VALIDATION" ]; then
            echo "⏳ Waiting for certificate validation..."
        fi
    else
        echo "Domain information not available yet..."
    fi
    
    sleep 60
done
