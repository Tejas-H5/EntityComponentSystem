import sys

tabLevel = 0

def newlineWithTabs():
	global tabLevel
	return "\n" + "".join(["\t" for i in range(tabLevel)])


def createMethodSignature(numGenericParams):
	functionDeclaration = "public int CreateEntity"

	templateArgs = "<T0"
	for i in range(1,numGenericParams):
		templateArgs += ", T" + str(i)
	templateArgs += ">"

	functionArgs = "(T0 c0"
	for i in range(1, numGenericParams):
		functionArgs += ", T" + str(i) + " c" + str(i)
	functionArgs += ")"

	global tabLevel
	tabLevel += 1
	functionArgs += newlineWithTabs()

	genericConstraints = ""
	for i in range(0, numGenericParams):
		genericConstraints += "where T" + str(i) + " : struct "

	tabLevel -= 1
	genericConstraints += newlineWithTabs()

	tabLevel += 1

	return functionDeclaration + templateArgs + functionArgs + genericConstraints


def createMethodBody(numGenericParams):
	global tabLevel

	methodBody = "{" + newlineWithTabs() + "int entity = CreateEntity();" + newlineWithTabs()
	
	for i in range(0, numGenericParams):
		methodBody += newlineWithTabs() + "addComponentNoEvent(entity, c" + str(i) + ");"
	
	methodBody += newlineWithTabs() + newlineWithTabs()
	methodBody += "invokeEntityCreatedEvent(entity);"
	methodBody += newlineWithTabs() + newlineWithTabs()
	methodBody += "return entity;"

	tabLevel -= 1
	methodBody += newlineWithTabs() + "}"
	return methodBody


def createExtensionMethod(numGenericParams):
	methodSignature = createMethodSignature(numGenericParams)
	methodBody = createMethodBody(numGenericParams)
	return methodSignature + methodBody


def prefixFileContent():
	global tabLevel
	content =  """
//This code was auto-generated with a python script. I hope you didn't think I would type all this out by hand
namespace ECS
{
	public partial class ECSWorld
	{"""

	tabLevel += 2
	return content + newlineWithTabs()

def suffixFileContent():
	global tabLevel
	content = """
	}
}
"""
	tabLevel -= 2
	return content


def createSourceFile(numGenericParams):
	file = prefixFileContent()
	file += createExtensionMethod(1)
	for i in range(2, numGenericParams+1):
		file += newlineWithTabs()
		file += newlineWithTabs()
		file += createExtensionMethod(i)

	file += suffixFileContent()
	return file


numGenericParams = int(sys.argv[1])

with open("ECSWorldCreateEntityExtensions.cs", "w") as file:
	file.write(createSourceFile(numGenericParams))