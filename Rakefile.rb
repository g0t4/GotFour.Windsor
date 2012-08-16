require 'rake'
require 'albacore'

$projectSolution = 'src/GotFour.Windsor.sln'
$artifactsPath = "build"
$nugetFeedPath = ENV["NuGetDevFeed"]
$srcPath = "src"

task :teamcity => [:build_release]

task :build => [:build_release]

msbuild :build_release => [:clean, :dep] do |msb|
  msb.properties :configuration => :Release
  msb.targets :Build
  msb.solution = $projectSolution
end

desc "Clean the repository before a build"
task :clean do
    puts "Cleaning"
    FileUtils.rm_rf $artifactsPath
	bins = FileList[File.join($srcPath, "**/bin")].map{|f| File.expand_path(f)}
	bins.each do |file|
		sh %Q{rmdir /S /Q "#{file}"}
		#sh 
    end
end

task :nuget => [:build] do
	sh "nuget pack src\\GotFour.Windsor\\GotFour.Windsor.csproj /OutputDirectory " + $nugetFeedPath
end