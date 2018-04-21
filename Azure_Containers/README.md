# Global Azure Container 2018

_Delivering modern cloud native applications with ​open source technologies on Azure​_

## Overview

This workshop will guide you through migrating an application from "on-premise" to containers running in Azure Kubernetes Service.

The labs are based upon a node.js application that allows for voting on the Justice League Superheroes (with more options coming soon). Data is stored in MongoDB.

> Note: They can be run locally on a Mac or Windows, but only for local machines that meet the requirements.

### Enable Service Dependencies

You may find that you need to add some dependencies to move foward with adding Kubernetes. The following commands will fix that.

`az provider register -n Microsoft.Network`
> Registering is still on-going. You can monitor using 'az provider show -n Microsoft.Network'

`az provider register -n Microsoft.Storage`
> Registering is still on-going. You can monitor using 'az provider show -n Microsoft.Storage'

`az provider register -n Microsoft.Compute`
> Registering is still on-going. You can monitor using 'az provider show -n Microsoft.Compute'

`az provider register -n Microsoft.ContainerService`
> Registering is still on-going. You can monitor using 'az provider show -n Microsoft.ContainerService'

## Contributing

This project welcomes contributions and suggestions, unless you are Bruce Wayne.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## License

This software is covered under the MIT license. You can read the license [here][license].

This software contains code from Heroku Buildpacks, which are also covered by the MIT license.

This software contains code from [Helm][], which is covered by the Apache v2.0 license.

You can read third-party software licenses [here][Third-Party Licenses].
