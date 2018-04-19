# IaC Labs

## Prerequisites

- [Azure PowerShell SDK](https://docs.microsoft.com/en-us/powershell/azure/install-azurerm-ps)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Visual Studio Code](https://code.visualstudio.com/)
- An Azure subscription
- [A flux capacitor](https://www.oreillyauto.com/flux-capacitor)

## 1. Create and deploy your first ARM template

Here's the [link to the tutorial](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-create-first-template).

This tutorial takes about 45 minutes.  You don't have to complete the entire tutorial, however, if you'd like to save time for the more advanced labs.

__Note__: Don't delete your newly-deployed Resource Groups if you would like to do the next lab.  You'll need some ARM deployments to review.

## 2. Reviewing / troubleshooting ARM deployments

This lab can take as few as 5 minutes, depending on how deep you go.

As mentioned in the presentation, the error messages returned from ARM template deployments sometimes take expertise to derive anything useful, if there's any useful info there at all.  The Azure portal provides more detail, and is usually more helpful.

Here are some [online instructions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-deployment-operations) on how to review ARM deployment details, both in the portal, as well as via the commandline using the Azure PowerShell SDK and Azure CLI functions that return the same deployment details.

## 3. Deploying other types of resources

There is a huge list of sample Azure deployments in the [Github ARM quickstart samples repo](https://github.com/Azure/azure-quickstart-templates).  Some of the samples are prefixed with 101, or 201, or 301 to indicate of expertise.  But you can often take one of the samples and have something deployed pretty quickly, depending on complexity.

## 4. Useful references

- [ARM template structure and syntax](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-authoring-templates)

- [ARM template functions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-template-functions)

- [ARM template quick starts](https://github.com/Azure/azure-quickstart-templates)  
   This is a great resource for examples of deploying many different resource types.

- [ARM template best practices](https://github.com/Azure/azure-quickstart-templates/blob/master/1-CONTRIBUTION-GUIDE/best-practices.md).

- [ARM template reference]((https://docs.microsoft.com/en-us/azure/templates/))  
  Here is the ultimate reference to the full template structure for all Azure resources (at least those that have ARM templates).  Just go to this page, then expand the Reference link in the lefthand pane.

## 5. IaC with Terraform

For some self-study, please consider reviewing [Terraform](https://www.terraform.io/) as another option for deploying resources to Azure.  It is a mature tool purpose-built for IaC deployments.  It's "mature" by open source standards, even though as of 4/2018 it's still only v0.11.7.  ;)

Note that while Terraform does not leverage ARM templates, it does talk to the ARM service using the Go Azure SDK.